using System;
using App2.Helpers;

namespace App2.Models
{
    public class BaseDataObject : ObservableObject
    {
        public BaseDataObject()
        {
            UUID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Id for item
        /// </summary>
        public string UUID { get; set; }

        /// <summary>
        /// Azure created at time stamp
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Azure UpdateAt timestamp for online/offline sync
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }

    }
}
