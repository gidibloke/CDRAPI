using Domain.LookupModels;

namespace CDRAPI.Interfaces
{
    public interface IUploadCallRecords
    {
        IList<Currency> Currencies { get; set; }

        Task<int> UploadRecords();
    }
}
