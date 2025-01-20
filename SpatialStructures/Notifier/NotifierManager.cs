using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpatialStructures.Notifier;

/// <summary>
/// Manages notification types by enabling or disabling specific notification flags.
/// </summary>
namespace SpatialStructures.Notifier
{
    public class NotifierManager
    {
        private ENotificationType _notifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifierManager"/> class.
        /// </summary>
        public NotifierManager() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifierManager"/> class with a specific notification type.
        /// </summary>
        /// <param name="type">The notification type to enable.</param>
        public NotifierManager(ENotificationType type)
        {
            Enable(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifierManager"/> class with multiple notification types.
        /// </summary>
        /// <param name="types">An array of notification types to enable.</param>
        public NotifierManager(ENotificationType[] types)
        {
            Enable(types);
        }

        /// <summary>
        /// Checks whether a specific notification type is enabled.
        /// </summary>
        /// <param name="type">The notification type to check.</param>
        /// <returns><c>true</c> if the notification type is enabled; otherwise, <c>false</c>.</returns>
        public bool IsEnabled(ENotificationType type)
        {
            return (_notifications & type) == type;
        }

        /// <summary>
        /// Enables a specific notification type.
        /// </summary>
        /// <param name="type">The notification type to enable.</param>
        public void Enable(ENotificationType type)
        {
            _notifications |= type;
        }

        /// <summary>
        /// Enables multiple notification types.
        /// </summary>
        /// <param name="types">An array of notification types to enable.</param>
        public void Enable(ENotificationType[] types)
        {
            foreach (ENotificationType type in types)
            {
                Enable(type);
            }
        }

        /// <summary>
        /// Disables a specific notification type.
        /// </summary>
        /// <param name="type">The notification type to disable.</param>
        public void Disable(ENotificationType type)
        {
            _notifications &= ~type;
        }

        /// <summary>
        /// Disables multiple notification types.
        /// </summary>
        /// <param name="types">An array of notification types to disable.</param>
        public void Disable(ENotificationType[] types)
        {
            foreach (ENotificationType type in types)
            {
                Disable(type);
            }
        }
    }
}

