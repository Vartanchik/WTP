using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.ModelsDto.History;

namespace WTP.BLL.ModelsDto.AdminOperation
{
    public enum OperationEnum
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Lock = 4,
        UnLock = 5
    }

    public class AdminOperationDto
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
