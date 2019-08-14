using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TemplateV2.Common.Notifications
{
    public class NotificationCollection : IEnumerable<Notification>
    {
        #region Instance Fields

        private List<Notification> _notifications;

        #endregion

        #region Properties

        public bool HasErrors
        {
            get
            {
                return _notifications.Any(n => n.Type == NotificationTypeEnum.Error);
            }
        }

        #endregion

        #region Constructors 

        public NotificationCollection()
        {
            _notifications = new List<Notification>();
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<Notification> GetEnumerator()
        {
            return _notifications.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _notifications.GetEnumerator();
        }


        #endregion

        #region Public Methods

        public void AddError(string error)
        {
            _notifications.Add(new Notification()
            {
                Message = error,
                Type = NotificationTypeEnum.Error
            });
        }

        public void AddErrors(List<string> errors)
        {
            foreach (var error in errors)
            {
                _notifications.Add(new Notification()
                {
                    Message = error,
                    Type = NotificationTypeEnum.Error
                });
            }
        }

        public void Add(NotificationCollection notifications)
        {
            _notifications.AddRange(notifications);
        }

        public void Add(string message, NotificationTypeEnum type)
        {
            _notifications.Add(new Notification()
            {
                Message = message,
                Type = type
            });
        }

        #endregion
    }
}
