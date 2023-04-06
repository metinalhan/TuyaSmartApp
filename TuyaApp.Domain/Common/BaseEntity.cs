using System;

namespace TuyaApp.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }      
        public bool IsActive { get; set; }
    }
}
