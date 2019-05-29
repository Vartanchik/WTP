﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.DAL.Entities.AppUserEntities
{
    public enum OperationEnum
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Lock = 4,
        UnLock = 5
    }

    public class Operation : IEntity
    {
        public int Id { get; set; }
        public OperationEnum OperationName
        {
            get => (OperationEnum)Id;
            set => Id = (int)value;
        }

        public List<History> Histories { get; set; }
    }

}
