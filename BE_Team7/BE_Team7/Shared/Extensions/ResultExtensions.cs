using GarageManagementAPI.Shared.ResultModel;

namespace BE_Team7.Shared.Extensions
{
    public static class ResultExtensions
    {
        public static TResultType GetValue<TResultType>(this Result result)
    => (result as Result<TResultType>)!.Value!;
    }
}
