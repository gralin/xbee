namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    public enum AssociationStatus
    {
        /// <summary>
        /// Successful completion - Coordinator started or Router/End Device found and joined with a parent.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Scan found no PANs
        /// </summary>
	    NoPan = 0x21,
	    /// <summary>
	    /// Scan found no valid PANs based on current SC and ID settings
	    /// </summary>
        NoValidPan = 0x22,
	    /// <summary>
	    /// Valid Coordinator or Routers found, but they are not allowing joining (NJ expired)
	    /// </summary>
        NjExpired = 0x23,
	    /// <summary>
	    /// Node Joining attempt failed (typically due to incompatible security settings)
	    /// </summary>
        NjFailed = 0x27,
        /// <summary>
        /// Coordinator Start attempt failed
        /// </summary>
	    CoordinatorStartFailed = 0x2a,
	    /// <summary>
	    /// Scanning for a Parent
	    /// </summary>
        ScanningForParent = 0xff,
        /// <summary>
        /// Checking for an existing coordinator
        /// </summary>
	    ExistingCoordinatorCheck = 0x2b
    }
}