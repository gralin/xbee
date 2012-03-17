namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    public enum PowerLevel
    {
        /// <summary>
        /// -10/10 dBm (-3 dB for XBee-PRO International variant).
        /// </summary>
        Level0 = 0,

        /// <summary>
        /// -6/12 dBm (-3 dB for XBee-PRO International variant).
        /// </summary>
        Level1 = 1,

        /// <summary>
        /// -4/14 dBm (2 dB for XBee-PRO International variant).
        /// </summary>
        Level2 = 2,

        /// <summary>
        /// -2/16 dBm (8 dB for XBee-PRO International variant).
        /// </summary>
        Level3 = 3,

        /// <summary>
        /// 0/18 dBm (10 dB for XBee-PRO International variant).
        /// </summary>
        Level4 = 4,
    }
}