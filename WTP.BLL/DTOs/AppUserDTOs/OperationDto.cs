using System;
using System.Collections.Generic;
using System.Text;
using WTP.DAL.Entities.AppUserEntities;

namespace WTP.BLL.DTOs.AppUserDTOs
{
    public class OperationDto
    {
        public int Id { get; set; }
        public OperationEnum OperationName
        {
            get => (OperationEnum)Id;
            set => Id = (int)value;
        }

        public List<HistoryDto> Histories { get; set; }
    }

}
