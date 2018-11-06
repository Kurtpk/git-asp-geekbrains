using System.Collections.Generic;

namespace WebStore.DomainNew.Dto
{
    public class SaveResult
    {
        public bool IsSuccess { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
