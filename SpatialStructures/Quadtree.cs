using SpatialStructures.Notifier;
using System;
using System.Drawing;
using System.Numerics;
using System.Reflection;

namespace SpatialStructures
{
    /// <summary>
    /// Represents a generic QuadTree data structure used for spatial partitioning.
    /// This structure can efficiently manage and query objects in a 2D space
    /// by organizing them into hierarchical quadrants.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the QuadTree. Objects must implement <see cref="ICollidable"/>.</typeparam>
    public class Quadtree<T> : IQuadtree<T> where T : ICollidable
    {
        private int?[] _id;
        private List<T> _objects;
        private Quadtree<T> _parent;
        private Quadtree<T>[] _subQuadrants;
        private Rectangle _bounds;
        private int _maxObjects;
        private int _maxLevel;
        private int _level;
        public int Count
        {
            get
            {
                int count = _objects.Count;
                if (_subQuadrants != null)
                {
                    foreach (var quadrant in _subQuadrants)
                    {
                        count += quadrant.Count;
                    }
                }
                return count;
            }
        }

        public Rectangle Size
        {
            get
            {
                return _bounds;
            }
        }

        private readonly object _lock = new object();

        private NotifierManager? _notifierManager = null;
        public event Action OnCheckCollision;
        public event Action<string> OnChanged;
        public event Action OnReDraw;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quadtree{T}"/> class.
        /// </summary>
        /// <param name="bound">The bounding rectangle of the Quadtree.</param>
        /// <param name="id">The unique identifier for this quadrant (optional).</param>
        /// <param name="level">The current level of the Quadtree (default is 0).</param>
        /// <param name="maxObjs">The maximum number of objects allowed before subdividing.</param>
        /// <param name="maxLvls">The maximum depth of the Quadtree.</param>
        /// <param name="parent">The parent Quadtree node, if any.</param>
        /// <param name="notificationTypes">Types of notifications to enable (optional).</param>
        public Quadtree(Rectangle bound, int id = 0, int level = 0, int maxObjs = 4, int maxLvls = 5, Quadtree<T> parent = null, ENotificationType[]? notificationTypes = null)
        {
            _id =
            [
                parent?._id[1], 
                id,
            ];
            _bounds = bound;
            _maxObjects = maxObjs;
            _level = level;
            _maxLevel = maxLvls;
            _objects = new List<T>();
            _subQuadrants = null;
            _parent = parent;

            if(notificationTypes != null)
            {
                _notifierManager = new NotifierManager(notificationTypes);
            }

            if(_parent != null)
            {
                this.OnCheckCollision += _parent.OnCheckCollision;
                this.OnReDraw += _parent.OnReDraw;
                this.OnChanged += _parent.OnChanged;
            }
        }

        /// <summary>
        /// Retrieves a list of all bounds (rectangles) of the current quadrant and its subquadrants.
        /// The method recursively gathers the bounds of the current quadrant and all its subquadrants.
        /// </summary>
        /// <returns>A list of rectangles representing the bounds of all quadrants.</returns>
        public List<Rectangle> GetAllBounds()
        {
            List<Rectangle> bounds = new List<Rectangle>
            {
                _bounds
            };

            if (_subQuadrants != null)
            {
                foreach (var quadrant in _subQuadrants)
                {
                    bounds.AddRange(quadrant.GetAllBounds());
                }
            }

            return bounds;
        }

        /// <summary>
        /// Clears all objects and subquadrants from the current quadrant. 
        /// This method clears the objects in the current quadrant and recursively clears all subquadrants.
        /// </summary>
        public void Clear()
        {
            _objects.Clear();
            if (_subQuadrants != null)
            {
                foreach (var quadrant in _subQuadrants)
                {
                    quadrant.Clear();
                }
                _subQuadrants = null;
            }
            string parentInfo = _parent != null ? $" from parent {_parent._id[0]}" : "";
            string message = $"All objects from quadrant {this._id[1]} was removed{parentInfo}.";
            NotifyChange(ENotificationType.OnSubscribe, message);
        }

        /// <summary>
        /// Inserts an object into the Quadtree.
        /// If the object does not entirely fit into one sub-quadrant, it is stored in the current quadrant.
        /// </summary>
        /// <param name="obj">The object to insert.</param>
        /// <param name="objBounds">The bounds of the object.</param>
        public void Insert(T obj, Rectangle objBounds)
        {
            if(_subQuadrants != null)
            {
                int index = GetSubQuadrantIndex(objBounds);
                if(index != -1)
                {
                    _subQuadrants[index].Insert(obj, objBounds);
                    NotifyChange(ENotificationType.OnInsert, $"{obj} was inserted into subcuadrant {index} from the parent {this._id[1]}");
                    return;
                }
            }
           
            _objects.Add(obj);
            obj.IsActive = true;
            SubscribeObject(obj);

            if(_objects.Count > _maxObjects && _level < _maxLevel)
            {
                if(_subQuadrants == null) Subdivide();

                RedistributeObjects();
            }

            NotifyChange(ENotificationType.OnInsert, $"{obj} was inserted into quadrant {this._id[1]}");
        }


        /// <summary>
        /// Removes an object from the Quadtree.
        /// </summary>
        /// <param name="obj">The object to remove.</param>
        /// <returns>True if the object was removed successfully; otherwise, false.</returns>
        public bool Remove(T obj)
        {
            lock(_lock)
            {
                UnsubscribeObject(obj);
                obj.IsActive = false;
                NotifyChange(ENotificationType.OnRemove, $"{obj} was removed from quadrante {this._id[1]}");

                if (_parent != null)
                {
                    return _parent.Remove(obj);
                }

                if (_subQuadrants != null && RemoveObjectFromSubQuadrants(obj, _subQuadrants))
                {
                    return true;
                }

                return _objects.Remove(obj);

                bool RemoveObjectFromSubQuadrants(T obj, Quadtree<T>[] quadtrees)
                {
                    foreach (var sub in quadtrees)
                    {
                        if (sub._objects.Remove(obj))
                        {
                            return true;
                        }

                        if (sub._subQuadrants != null && RemoveObjectFromSubQuadrants(obj, sub._subQuadrants))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }     
        }

        /// <summary>
        /// Re-inserts an object into the Quadtree.
        /// The object is first removed and then reinserted based on its updated bounds.
        /// </summary>
        /// <param name="obj">The object to reinsert.</param>
        public void ReinsertObject(T obj)
        {
            lock (_lock)
            {
                if (!obj.IsReinserting && !Remove(obj))
                {
                    throw new InvalidOperationException("El objeto no se encuentra en el Quadtree.");
                }

                obj.IsReinserting = true;

                Rectangle newBounds = GetBoundsOfObject(obj);

                if (_parent != null && !_bounds.Contains(newBounds))
                {
                    _parent.ReinsertObject(obj);
                }
                else
                {
                    Insert(obj, newBounds);
                    NotifyChange(ENotificationType.OnObjectReinserted, $"{obj} was Reinserted");
                }

                obj.IsReinserting = false;

                this.OnCheckCollision?.Invoke();
            }
        }

        /// <summary>
        /// Retrieves a list of objects of type <c>T</c> that are within the specified area (Rectangle). 
        /// The method searches the current quadrant and its subquadrants for objects matching the given area.
        /// </summary>
        /// <param name="area">The area to search for objects within.</param>
        /// <returns>A list of objects found within the specified area.</returns>
        public List<T> Retrieve(Rectangle area)
        {
            List<T> foundObjects = [];

            if (_subQuadrants != null)
            {
                int index = GetSubQuadrantIndex(area);
                if (index != -1)
                {
                    foundObjects.AddRange(_subQuadrants[index].Retrieve(area));
                }
                else
                {
                    foreach(var sub in _subQuadrants)
                    {
                        foundObjects.AddRange(sub.Retrieve(area));
                    }
                }
            }

            foundObjects.AddRange(_objects);
            return foundObjects;
        }

        /// <summary>
        /// Retrieves a list of <c>Rectangle</c> objects representing the bounds of all objects 
        /// in the current quadrant and its subquadrants. 
        /// </summary>
        /// <returns>A list of rectangles representing the bounds of objects in the current quadrant.</returns>
        public List<Rectangle> Retrieve()
        {
            List<Rectangle> foundObjects = [];

            if (_subQuadrants != null)
            {
                foreach (var sub in _subQuadrants)
                {
                    var subObjects = sub.Retrieve();
                    if (subObjects != null && subObjects.Count > 0)
                    {
                        foundObjects.AddRange(subObjects);
                    }
                }
            }

            if (_objects != null && _objects.Count > 0)
            {
                foundObjects.Add(_bounds);
            }

            return foundObjects;
        }

        /// <summary>
        /// Resizes the Quadtree to a new area, redistributing all stored objects.
        /// </summary>
        /// <param name="newWidth">The new width of the Quadtree's bounds.</param>
        /// <param name="newHeight">The new height of the Quadtree's bounds.</param>
        public void Resize(int newWidth, int newHeight) 
        {
            _bounds = new Rectangle(0, 0, newWidth, newHeight);

            NotifyChange(ENotificationType.OnResize, $"Quadtree was resized into new parameters: Width: {newWidth}, Hewight: {newHeight}");

            List<T> allObjects = new List<T>();
            GatherObjects(allObjects);

            _subQuadrants = null;
            _objects.Clear();

            foreach (var obj in allObjects) 
            {
                Rectangle objBounds = GetBoundsOfObject(obj);
                Insert(obj, objBounds);
            }

            NeedReDraw();
        }

        /// <summary>
        /// Subdivides the current node into four equal sub-quadrants.
        /// </summary>
        public void Subdivide()
        {
            int subWidth = _bounds.Width / 2;
            int subHeight = _bounds.Height / 2;

            _subQuadrants = new Quadtree<T>[4];

            for (int i = 0; i < 4; i++)
            {
                int x = _bounds.X + (i % 2) * subWidth; 
                int y = _bounds.Y + (i / 2) * subHeight;

                _subQuadrants[i] = new Quadtree<T>(
                    new Rectangle(x, y, subWidth, subHeight),
                    i + 1,
                    _level + 1,
                    _maxObjects,
                    _maxLevel,
                    this
                );
            }

            NotifyChange(ENotificationType.OnSubdivide, $"Quadrant {this._id[1]} was subdivided");
            NeedReDraw();
        }

        /// <summary>
        /// Redistributes objects in the quadtree by moving them to the appropriate subquadrants 
        /// based on their current bounds. Objects that belong in a subquadrant are moved from 
        /// the main list to the relevant subquadrant.
        /// </summary>
        private void RedistributeObjects()
        {
            var objectsToMove = new List<(T obj, Rectangle bounds)>();

            foreach (var obj in _objects)
            {
                var bounds = GetBoundsOfObject(obj);
                int index = GetSubQuadrantIndex(bounds);
                if (index != -1)
                {
                    objectsToMove.Add((obj, bounds));
                }
            }

            foreach (var (obj, bounds) in objectsToMove)
            {
                _subQuadrants[GetSubQuadrantIndex(bounds)].Insert(obj, bounds);
                _objects.Remove(obj);
            }
        }


        /// <summary>
        /// Determines which sub-quadrant the specified bounds belong to.
        /// </summary>
        /// <param name="objBounds">The bounds of the object to check.</param>
        /// <returns>The index of the sub-quadrant (0-3), or -1 if it does not fit entirely in one sub-quadrant.</returns>
        private int GetSubQuadrantIndex(Rectangle objBounds)
        {
            int midX = _bounds.X + _bounds.Width / 2;
            int midY = _bounds.Y + _bounds.Height / 2;

            bool topQuadrant = objBounds.Y < midY && objBounds.Y + objBounds.Height <= midY;
            bool bottomQuadrant = objBounds.Y >= midY;

            if (objBounds.X < midX && objBounds.X + objBounds.Width <= midX)
            {
                if (topQuadrant) return 0;    // Top-left
                if (bottomQuadrant) return 2; // Bottom-left
            }
            else if (objBounds.X >= midX)
            {
                if (topQuadrant) return 1;    // Top-right
                if (bottomQuadrant) return 3; // Bottom-right
            }

            return -1;
        }

        /// <summary>
        /// Collects all objects from this node and its sub-quadrants into a single list.
        /// </summary>
        /// <param name="allObjects">The list to collect objects into.</param>
        private void GatherObjects(List<T> allObjects)
        {
            allObjects.AddRange(_objects);

            if (_subQuadrants != null)
            {
                foreach(var sub in _subQuadrants)
                {
                    sub.GatherObjects(allObjects);
                }
            }
        }

        /// <summary>
        /// Gets the bounds of an object, assuming the object implements IHasBounds.
        /// </summary>
        /// <param name="obj">The object to get bounds for.</param>
        /// <returns>The bounds of the object.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the object does not implement IHasBounds.</exception>
        private Rectangle GetBoundsOfObject(T obj)
        {
            if (obj is ICollidable hasArea)
            {
                return hasArea.Area;
            }

            throw new InvalidOperationException("Objects must implement IHasBounds to be used with QuadTree.");
        }

        /// <summary>
        /// Handles the position change of an object in the quadtree. It checks for conditions that might prevent 
        /// the position change, repositions the object if necessary, and notifies the relevant changes.
        /// </summary>
        /// <param name="sender">The object whose position is changing.</param>
        /// <param name="e">Event arguments associated with the position change.</param>
        private void HandleObjectPositionChanged(object sender, EventArgs e)
        {
            T obj = (T)sender;

            string errorMessage = "The object position can not change because";

            string? reason = obj.IsReinserting ? "object is being reinserted." :
                           !obj.IsActive ? "object is not active into quadtree." :
                           null;

            if (reason != null)
            {
                NotifyChange(ENotificationType.OnObjectPositionCanNotChange, $"{errorMessage} {reason}");
                return;
            }

            Rectangle newBounds = GetBoundsOfObject(obj);

            lock (_lock)
            {
                if (!_bounds.Contains(newBounds))
                {
                    ReinsertObject(obj);
                    return;
                }

                int newIndex = GetSubQuadrantIndex(newBounds);
                if (newIndex != -1 && _subQuadrants != null)
                {
                    if (_subQuadrants[newIndex]._objects.Contains(obj)) return;

                    Remove(obj);
                    _subQuadrants[newIndex].Insert(obj, newBounds);
                }
                else
                {
                    if (!_objects.Contains(obj))
                    {
                        _objects.Add(obj);
                    }
                }
            }

            NotifyChange(ENotificationType.OnObjectPositionChange, "The object position had been changed.");
        }

        /// <summary>
        /// Subscribes an object to the OnAreaChanged event and logs the subscription details.
        /// This allows the object to trigger position change handling when its area is modified.
        /// </summary>
        /// <param name="obj">The object to be subscribed to the event.</param>
        private void SubscribeObject(T obj)
        {
            obj.OnAreaChanged += HandleObjectPositionChanged;
            string parentInfo = _parent != null ? $" from parent {_parent._id[0]}" : "";
            string message = $"The object \"{obj}\" was subscribed to OnAreaChanged event from quadrant {this._id[1]}{parentInfo}.";
            NotifyChange(ENotificationType.OnSubscribe, message);
        }

        /// <summary>
        /// Unsubscribes an object from the OnAreaChanged event and logs the unsubscription details.
        /// This stops the object from triggering position change handling when its area is modified.
        /// </summary>
        /// <param name="obj">The object to be unsubscribed from the event.</param>
        private void UnsubscribeObject(T obj)
        {
            obj.OnAreaChanged -= HandleObjectPositionChanged;
            string parentInfo = _parent != null ? $" from parent {_parent._id[0]}" : "";
            string message = $"The object \"{obj}\" was subscribed to OnAreaChanged event from quadrant {this._id[1]}{parentInfo}.";
            NotifyChange(ENotificationType.OnUnsuscribe, message);
        }

        /// <summary>
        /// Notifies listeners about a change in the Quadtree's state.
        /// </summary>
        /// <param name="type">The type of notification.</param>
        /// <param name="message">The message describing the change.</param>
        private void NotifyChange(ENotificationType type, string message)
        {
            if (_notifierManager == null) return;

            if (_notifierManager.IsEnabled(type))
            {
                string notification = $"{type.ToString()}: {message}";
                OnChanged?.Invoke(notification);
            }
        }

        /// <summary>
        /// Triggers the redraw event for the Quadtree.
        /// This can be used to notify external listeners that the Quadtree needs to be visually updated.
        /// </summary>
        private void NeedReDraw()
        {
            OnReDraw?.Invoke();
        }

    }
}
