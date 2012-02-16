using System;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
  /// <summary>
  /// AT commands.
  /// </summary>
  /// <remarks>
  /// Summary descriptions of these commands are copied directly from
  /// Product Manual v1.xEx - 802.15.4 Protocol located at 
  /// http://ftp1.digi.com/support/documentation/90000982_F.pdf
  /// </remarks>
  public enum AtCmds
  {
    /// <summary>
    /// Write. Write parameter values to non-volatile memory so that parameter modifications
    /// persist through subsequent power-up or reset.
    /// </summary>
    /// <remarks>
    /// Once WR is issued, no additional characters should be sent to the module until
    /// after the response "OK\r" is received.
    /// </remarks>
    WR,
    
    /// <summary>
    /// Restore Defaults. Restore module parameters to factory defaults.
    /// </summary>
    RE,
    
    /// <summary>
    ///  Software Reset. Responds immediately with an OK then performs a hard reset
    /// ~100ms later.   
    /// </summary>
    /// <remarks>
    /// Introduced in firmware v1.x80.
    /// </remarks>
    FR,
    
    /// <summary>
    /// Channel. Set/Read the channel number used for transmitting and receiving data
    /// between RF modules (uses 802.15.4 protocol channel numbers).  
    /// </summary>
    CH,

    /// <summary>
    /// PAN ID. Set/Read the PAN (Personal Area Network) ID.   
    /// </summary>
    /// <remarks>
    /// Use 0xFFFF to broadcast messages to all PANs. 
    /// </remarks>
    ID,

    /// <summary>
    /// Destination Address High. Set/Read the upper 32 bits of the 64-bit destination
    /// address. 
    /// </summary>
    /// <remarks>
    /// When combined with DL, it defines the destination address used for
    /// transmission. To transmit using a 16-bit address, set DH parameter to zero and DL less
    /// than 0xFFFF.  
    /// </remarks>
    DH,

    /// <summary>
    /// Destination Address Low. Set/Read the lower 32 bits of the 64-bit destination
    /// address.  
    /// </summary>
    /// <remarks>
    /// When combined with DH, DL defines the destination address used for
    /// transmission. To transmit using a 16-bit address, set DH parameter to zero and DL less
    /// than 0xFFFF. 
    /// </remarks>
    DL,

    /// <summary>
    ///     
    /// </summary>
    MY,

    /// <summary>
    ///     
    /// </summary>
    SH,

    /// <summary>
    ///     
    /// </summary>
    SL,

    /// <summary>
    ///     
    /// </summary>
    RR,

    /// <summary>
    ///     
    /// </summary>
    RN,

    /// <summary>
    ///     
    /// </summary>
    MM,

    /// <summary>
    ///     
    /// </summary>
    NI,

    /// <summary>
    ///     
    /// </summary>
    ND,

    /// <summary>
    ///     
    /// </summary>
    NT,

    /// <summary>
    ///     
    /// </summary>
    NO,

    /// <summary>
    ///     
    /// </summary>
    DN,

    /// <summary>
    ///     
    /// </summary>
    CE,

    /// <summary>
    ///     
    /// </summary>
    SC,

    /// <summary>
    ///     
    /// </summary>
    SD,

    /// <summary>
    ///     
    /// </summary>
    A1,

    /// <summary>
    ///     
    /// </summary>
    A2,

    /// <summary>
    ///     
    /// </summary>
    AI,

    /// <summary>
    ///     
    /// </summary>
    DA,

    /// <summary>
    ///     
    /// </summary>
    FP,

    /// <summary>
    ///     
    /// </summary>
    AS,

    /// <summary>
    ///     
    /// </summary>
    ED,

    /// <summary>
    ///     
    /// </summary>
    EE,

    /// <summary>
    ///     
    /// </summary>
    KY,

    /// <summary>
    ///     
    /// </summary>
    PL,

    /// <summary>
    ///     
    /// </summary>
    CA,

    /// <summary>
    ///     
    /// </summary>
    SM,

    /// <summary>
    ///     
    /// </summary>
    SO,

    /// <summary>
    ///     
    /// </summary>
    ST,

    /// <summary>
    ///     
    /// </summary>
    SP,

    /// <summary>
    ///     
    /// </summary>
    DP,

    /// <summary>
    ///     
    /// </summary>
    BD,

    /// <summary>
    ///     
    /// </summary>
    RO,

    /// <summary>
    ///     
    /// </summary>
    AP,

    /// <summary>
    ///     
    /// </summary>
    NB,

    /// <summary>
    ///     
    /// </summary>
    PR,

    /// <summary>
    ///     
    /// </summary>
    D7,

    /// <summary>
    ///     
    /// </summary>
    D6,

    /// <summary>
    ///     
    /// </summary>
    D5,

    /// <summary>
    ///     
    /// </summary>
    D4,

    /// <summary>
    ///     
    /// </summary>
    D3,

    /// <summary>
    ///     
    /// </summary>
    D2,

    /// <summary>
    ///     
    /// </summary>
    D1,

    /// <summary>
    ///     
    /// </summary>
    D0,

    /// <summary>
    ///     
    /// </summary>
    IU,

    /// <summary>
    ///     
    /// </summary>
    IT,

    /// <summary>
    ///     
    /// </summary>
    IS,

    /// <summary>
    ///     
    /// </summary>
    IO,

    /// <summary>
    ///     
    /// </summary>
    IC,

    /// <summary>
    ///     
    /// </summary>
    IR,

    /// <summary>
    ///     
    /// </summary>
    IA,

    /// <summary>
    ///     
    /// </summary>
    T0,

    /// <summary>
    ///     
    /// </summary>
    T1,

    /// <summary>
    ///     
    /// </summary>
    T2,

    /// <summary>
    ///     
    /// </summary>
    T3,

    /// <summary>
    ///     
    /// </summary>
    T4,

    /// <summary>
    ///     
    /// </summary>
    T5,

    /// <summary>
    ///     
    /// </summary>
    T6,

    /// <summary>
    ///     
    /// </summary>
    T7,

    /// <summary>
    ///     
    /// </summary>
    P0,

    /// <summary>
    ///     
    /// </summary>
    P1,

    /// <summary>
    ///     
    /// </summary>
    M0,

    /// <summary>
    ///     
    /// </summary>
    M1,

    /// <summary>
    ///     
    /// </summary>
    PT,

    /// <summary>
    ///     
    /// </summary>
    RP,

    /// <summary>
    ///     
    /// </summary>
    VR,

    /// <summary>
    ///     
    /// </summary>
    VL,

    /// <summary>
    ///     
    /// </summary>
    HV,

    /// <summary>
    ///     
    /// </summary>
    DB,

    /// <summary>
    ///     
    /// </summary>
    EC,

    /// <summary>
    ///     
    /// </summary>
    EA,

    /// <summary>
    ///     
    /// </summary>
    CT,

    /// <summary>
    ///     
    /// </summary>
    CN,

    /// <summary>
    ///     
    /// </summary>
    AC,

    /// <summary>
    ///     
    /// </summary>
    GT,

    /// <summary>
    ///     
    /// </summary>
    GC
  }


  /// <summary>
  /// AT Command
  /// </summary>
  /// <remarks>
  /// Implemented to wrap the AtCmds enumeration so
  /// the actual string value can be retrieved. NETMF
  /// does not support getting the actual string value
  /// of the enumeration member name directly.
  /// </remarks>
  public struct AtCmd
  {
    private readonly AtCmds _cmd;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="cmd" type="Gadgeteer.Modules.GHIElectronics.Api.AtCmds">
    ///     <para>
    ///     AT Command enumeration value.    
    ///     </para>
    /// </param>
    public AtCmd(AtCmds cmd)
    {
      _cmd = cmd;
    }

    /// <summary>
    /// AtCmds enumeration value.
    /// </summary>
    public AtCmds Cmd
    {
      get { return _cmd; }
    }

    /// <summary>
    /// Override.
    /// </summary>
    /// <returns>
    /// Returns the name of the AtCmds
    /// enumeration value.    
    /// </returns>
    public override string ToString()
    {
      string str;
      switch (Cmd)
      {
        case AtCmds.WR: str = "WR"; break;
        case AtCmds.RE: str = "RE"; break;
        case AtCmds.FR: str = "FR"; break;
        case AtCmds.CH: str = "CH"; break;
        case AtCmds.ID: str = "ID"; break;
        case AtCmds.DH: str = "DH"; break;
        case AtCmds.DL: str = "DL"; break;
        case AtCmds.MY: str = "MY"; break;
        case AtCmds.SH: str = "SH"; break;
        case AtCmds.SL: str = "SL"; break;
        case AtCmds.RR: str = "RR"; break;
        case AtCmds.RN: str = "RN"; break;
        case AtCmds.MM: str = "MM"; break;
        case AtCmds.NI: str = "NI"; break;
        case AtCmds.ND: str = "ND"; break;
        case AtCmds.NT: str = "NT"; break;
        case AtCmds.NO: str = "NO"; break;
        case AtCmds.DN: str = "DN"; break;
        case AtCmds.CE: str = "CE"; break;
        case AtCmds.SC: str = "SC"; break;
        case AtCmds.SD: str = "SD"; break;
        case AtCmds.A1: str = "A1"; break;
        case AtCmds.A2: str = "A2"; break;
        case AtCmds.AI: str = "AI"; break;
        case AtCmds.DA: str = "DA"; break;
        case AtCmds.FP: str = "FP"; break;
        case AtCmds.AS: str = "AS"; break;
        case AtCmds.ED: str = "ED"; break;
        case AtCmds.EE: str = "EE"; break;
        case AtCmds.KY: str = "KY"; break;
        case AtCmds.PL: str = "PL"; break;
        case AtCmds.CA: str = "CA"; break;
        case AtCmds.SM: str = "SM"; break;
        case AtCmds.SO: str = "SO"; break;
        case AtCmds.ST: str = "ST"; break;
        case AtCmds.SP: str = "SP"; break;
        case AtCmds.DP: str = "SP"; break;
        case AtCmds.BD: str = "BD"; break;
        case AtCmds.RO: str = "RO"; break;
        case AtCmds.AP: str = "AP"; break;
        case AtCmds.NB: str = "NB"; break;
        case AtCmds.PR: str = "PR"; break;
        case AtCmds.D7: str = "D7"; break;
        case AtCmds.D6: str = "D6"; break;
        case AtCmds.D5: str = "D5"; break;
        case AtCmds.D4: str = "D4"; break;
        case AtCmds.D3: str = "D3"; break;
        case AtCmds.D2: str = "D2"; break;
        case AtCmds.D1: str = "D1"; break;
        case AtCmds.D0: str = "D0"; break;
        case AtCmds.IU: str = "IU"; break;
        case AtCmds.IT: str = "IT"; break;
        case AtCmds.IS: str = "IS"; break;
        case AtCmds.IO: str = "IO"; break;
        case AtCmds.IC: str = "IC"; break;
        case AtCmds.IR: str = "IR"; break;
        case AtCmds.IA: str = "IA"; break;
        case AtCmds.T0: str = "T0"; break;
        case AtCmds.T1: str = "T1"; break;
        case AtCmds.T2: str = "T2"; break;
        case AtCmds.T3: str = "T3"; break;
        case AtCmds.T4: str = "T4"; break;
        case AtCmds.T5: str = "T5"; break;
        case AtCmds.T6: str = "T6"; break;
        case AtCmds.T7: str = "T7"; break;
        case AtCmds.P0: str = "P0"; break;
        case AtCmds.P1: str = "P1"; break;
        case AtCmds.M0: str = "M0"; break;
        case AtCmds.M1: str = "M1"; break;
        case AtCmds.PT: str = "PT"; break;
        case AtCmds.RP: str = "RP"; break;
        case AtCmds.VR: str = "VR"; break;
        case AtCmds.VL: str = "VL"; break;
        case AtCmds.HV: str = "HV"; break;
        case AtCmds.DB: str = "DB"; break;
        case AtCmds.EC: str = "EC"; break;
        case AtCmds.EA: str = "EA"; break;
        case AtCmds.CT: str = "CT"; break;
        case AtCmds.CN: str = "CN"; break;
        case AtCmds.AC: str = "AC"; break;
        case AtCmds.GT: str = "GT"; break;
        case AtCmds.GC: str = "GC"; break;
        default:
          throw new ArgumentException("AT Command " + _cmd + " not supported.");
          break;
      }
      return str;
    }

  }

    /// <summary>
    /// API technique to set/query commands
    /// </summary>
    /// <remarks>
    /// WARNING: Any changes made will not survive a power cycle unless written to memory with WR command
    /// According to the manual, the WR command can only be written so many times.. however many that is.
    /// <para>
    /// API ID: 0x8
    /// </para>
    /// Determining radio type with HV:
    /// <list type="table">
    ///     <listheader>
    ///         <term>Byte 1</term>
    ///         <description>Part Number</description>
    ///     </listheader>  
    ///     <item>
    ///         <term>x17</term>
    ///         <description>XB24 (series 1)</description>
    ///     </item>
    ///     <item>
    ///         <term>x18</term>
    ///         <description>XBP24 (series 1)</description>
    ///     </item>
    ///     <item>
    ///         <term>x19</term>
    ///         <description>XB24-B (series 2</description>
    ///     </item>
    ///     <item>
    ///         <term>x1A</term>
    ///         <description>XBP24-B (series 2)</description>
    ///     </item>
    /// </list>
    /// XB24-ZB
    /// XBP24-ZB
    /// </remarks>
    public class AtCommand : XBeeRequest
    {

        public string Command { get; set; }
        public int[] Value { get; set; }

        public AtCommand(string command) 
            : this (command, null, DEFAULT_FRAME_ID)
        {
        }

        public AtCommand(AtCmds command)
          : this(command.ToString(), null, DEFAULT_FRAME_ID)
        {
        }

        public AtCommand(string command, int value) 
            : this (command, new[]{value}, DEFAULT_FRAME_ID)
        {
        }

        public AtCommand(AtCmds command, int value)
          : this(command.ToString(), new[] { value }, DEFAULT_FRAME_ID)
        {
        }

        public AtCommand(string command, int[] value)
            : this(command, value, DEFAULT_FRAME_ID)
        {
        }

        public AtCommand(AtCmds command, int[] value)
          : this(command.ToString(), value, DEFAULT_FRAME_ID)
        {
        }

        /// <summary></summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <param name="frameId">frameId must be > 0 for a response</param>
        public AtCommand(string command, int[] value, int frameId)
        {
            Command = command;
            Value = value;
            FrameId = frameId;
        }

        public AtCommand(AtCmds command, int[] value, int frameId)
        {
          Command = command.ToString();
          Value = value;
          FrameId = frameId;
        }

      public override int[] GetFrameData()
        {
            if (Command.Length > 2)
                throw new ArgumentException("Command should be two characters. Do not include AT prefix");

            var frameData = new IntArrayOutputStream();

            frameData.Write((byte) ApiId);
            frameData.Write(FrameId);
            frameData.Write(Command[0]);
            frameData.Write(Command[1]);

            if (Value != null)
                frameData.Write(Value);

            return frameData.GetIntArray();
        }

        public override ApiId ApiId
        {
            get { return ApiId.AT_COMMAND; }
        }

        public override string ToString()
        {
            return base.ToString() 
                + ",command=" + Command 
                + ",value=" + (Value == null ? "null" : ByteUtils.ToBase16(Value));
        }
    }


}