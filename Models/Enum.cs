using System.ComponentModel.DataAnnotations;

namespace FridgeManagementSystem.Models
{
    public enum LightingStatus
    {
        
        Functional,

        
        Replace,

       
        Flickering,

       
        NotWorking,

        
        BulbBroken
    }
    public enum DoorSealCondition
    {
       
        SealingProperty,

       
        WornOut,

       
        Damaged,

       
        Missing,

        
        Loose
    }
    public enum CoolantLevel
    {
        
        OK,

       
        Low,

        
        Leaking,

        
        Empty,

       
        Overfilled
    }
    public enum TemperatureStatus
    {
       
        Normal,

        
        High,

        
        Low,

        
        Fluctuating,

        
        NotCooling
    }
    public enum ComponentCondition
    {
       
        New,

       
        Used,

        
        Refurbished,

        
        Reconditioned
    }
    public enum UrgencyLevel
    {
       
        Low,

        
        Medium,

       
        High,

        
        Critical,

        Emergency
    }
    public enum FaultType
    {
        
        Electrical,

        
        DoorSeal,

        
        CoolingIssue,

        
        NoiseVibration,

        
        PowerIssue,

        
        Compressor,

        
        Thermostat,

        
        WaterLeak,

       
        IceMaker,

       
    }
    public enum TaskStatus
    {
        
        Pending=0,

       
        InProgress=1,

        
        Complete=2,

        
        Cancelled=3,

       
        Scheduled=4,

       
        OnHold=5,

       
        Rescheduled=6
    }
    public enum PowerCableCondition
    {
       
        Good,

        
        Frayed,

        
        NeedsReplacement,

        
        Damaged,

       
        LooseConnection
    }
    public enum ShopType
    {
        Shebeen ,
        Warehouse,
        Restuarant,
        SuperMarket,
        Spaza
    }
}
