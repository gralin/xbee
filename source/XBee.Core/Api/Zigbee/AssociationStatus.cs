namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    public enum AssociationStatus
    {
        /// <summary>
        /// Successful completion - Coordinator started or Router/End Device found and joined with a parent.
        /// </summary>
        SUCCESS = 0,
        /// <summary>
        /// Scan found no PANs
        /// </summary>
	    NO_PAN = 0x21,
	    /// <summary>
	    /// Scan found no valid PANs based on current SC and ID settings
	    /// </summary>
        NO_VALID_PAN = 0x22,
	    /// <summary>
	    /// Valid Coordinator or Routers found, but they are not allowing joining (NJ expired)
	    /// </summary>
        NJ_EXPIRED = 0x23,
	    /// <summary>
	    /// Node Joining attempt failed (typically due to incompatible security settings)
	    /// </summary>
        NJ_FAILED = 0x27,
        /// <summary>
        /// Coordinator Start attempt failed
        /// </summary>
	    COORDINATOR_START_FAILED = 0x2a,
	    /// <summary>
	    /// Scanning for a Parent
	    /// </summary>
        SCANNING_FOR_PARENT = 0xff,
        /// <summary>
        /// Checking for an existing coordinator
        /// </summary>
	    EXISTING_COORDINATOR_CHECK = 0x2b
    }
}