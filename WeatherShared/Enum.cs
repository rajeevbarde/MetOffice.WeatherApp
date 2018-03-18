namespace WeatherShared
{
    /// <summary>
    /// MetOffice Region
    /// </summary>
    public enum Region
    {
        ///<summary>United Kingdom</summary>
        UK,
        ///<summary>England</summary>
        England,
        ///<summary>Wales</summary>
        Wales,
        ///<summary>Scotland</summary>
        Scotland
    };

    /// <summary>
    /// Region's climate types
    /// </summary>
    public enum ClimateType
    {
        ///<summary>Maximum Temperature</summary>
        max,
        ///<summary>Minimum Temperature</summary>
        min,
        ///<summary>Mean Temperature</summary>
        mean,
        ///<summary>Sunshine details</summary>
        sunshine,
        ///<summary>Rainfall details</summary>
        rainfall
    };
}
