namespace Api.Dtos
{
    internal record DataClimate(
        int DatasourceId, 
        string Format, 
        int IntervalMs, 
        int MaxDataPoints, 
        string RefId, 
        string RawSql);
}
