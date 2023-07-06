using Business_AnimeToNotion.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_AnimeToNotion.Model.Internal
{
    public class INT_AnimeShowEdit
    {
        public Guid? Id { get; set; }
        public string? Status { get; set; }
        public int? PersonalScore { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? FinishedOn { get; set; }
        public string? Notes { get; set; }
        public CompletedYear? CompletedYear { get; set; }
    }
}
