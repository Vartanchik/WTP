using System;
using System.Collections.Generic;
using System.Text;
using WTP.BLL.ModelsDto.History;
using WTP.DAL.DomainModels;

namespace WTP.BLL.ModelsDto.Operation
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
