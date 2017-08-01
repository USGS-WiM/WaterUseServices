//------------------------------------------------------------------------------
//----- Equality ---------------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2017 WiM - USGS

//    authors:  Jeremy K. Newson USGS Web Informatics and Mapping
//              
//  
//   purpose:   Overrides Equatable
//
//discussion:   https://blogs.msdn.microsoft.com/ericlippert/2011/02/28/guidelines-and-rules-for-gethashcode/    
//              http://www.aaronstannard.com/overriding-equality-in-dotnet/
//
//              var hashCode = 13;
//              hashCode = (hashCode * 397) ^ MyNum;
//              var myStrHashCode = !string.IsNullOrEmpty(MyStr) ?
//                                      MyStr.GetHashCode() : 0;
//              hashCode = (hashCode * 397) ^ MyStr;
//              hashCode = (hashCode * 397) ^ Time.GetHashCode();
//               
using System;
using System.Collections.Generic;
using System.Text;

namespace WaterUseDB.Resources
{
    public partial class CatagoryCoefficient : IEquatable<CatagoryCoefficient>
    {
        public bool Equals(CatagoryCoefficient other)
        {
            return this.CatagoryTypeID == other.CatagoryTypeID && this.RegionID == other.RegionID;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as CatagoryCoefficient);
        }
        public override int GetHashCode()
        {
            return (this.CatagoryTypeID + this.RegionID).GetHashCode();
        }
    }//end 

    public partial class CatagoryType : IEquatable<CatagoryType>
    {
        public bool Equals(CatagoryType other)
        {
            return String.Equals(this.Name.ToLower(),other.Name.ToLower()) && String.Equals(this.Code.ToLower(), other.Code.ToLower());
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as CatagoryType);
        }
        public override int GetHashCode()
        {
            return (this.Name + this.Code).GetHashCode();
        }
    }//end 

    public partial class Manager : IEquatable<Manager>
    {
        public bool Equals(Manager other)
        {
            return String.Equals(this.Username.ToLower(), other.Username.ToLower()) || (String.Equals(this.FirstName.ToLower(), other.FirstName.ToLower()) &&
                String.Equals(this.LastName.ToLower(), other.LastName.ToLower()) && 
                String.Equals(this.Email.ToLower(), other.Email.ToLower()));

        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Manager);
        }
        public override int GetHashCode()
        {
            return (this.Username+this.FirstName+this.LastName+Email).GetHashCode();
        }
    }//end 
    
    public partial class Permit : IEquatable<Permit>
    {
        public bool Equals(Permit other)
        {
            return String.Equals(this.PermitNO.ToLower(),other.PermitNO.ToLower());
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Permit);
        }
        public override int GetHashCode()
        {
            return (this.PermitNO).GetHashCode();
        }
    }//end 

    public partial class Region : IEquatable<Region>
    {
        public bool Equals(Region other)
        {
            return string.Equals(this.Name.ToLower(), other.Name.ToLower()) &&
            String.Equals(this.ShortName.ToLower(), other.ShortName.ToLower());
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Region);
        }
        public override int GetHashCode()
        {
            return (this.Name + this.ShortName).GetHashCode();
        }
    }//end 

    public partial class RegionManager : IEquatable<RegionManager>
    {
        public bool Equals(RegionManager other)
        {
            return this.RegionID == other.RegionID && this.ManagerID == other.ManagerID;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as RegionManager);
        }
        public override int GetHashCode()
        {
            return (this.RegionID + this.ManagerID).GetHashCode();
        }
    }//end     

    public partial class Role : IEquatable<Role>
    {
        public bool Equals(Role other)
        {
            return string.Equals(this.Name.ToLower(), other.Name.ToLower());
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as Role);
        }
        public override int GetHashCode()
        {
            return (this.Name).GetHashCode();
        }
    }//end

    public partial class Source : IEquatable<Source>
        {
            public bool Equals(Source other)
            {
                return String.Equals(this.FacilityName.ToLower(),other.FacilityName.ToLower()) && 
                       String.Equals(this.FacilityCode.ToLower(),other.FacilityCode.ToLower()) &&
                       this.SourceTypeID == other.SourceTypeID &&
                       this.RegionID == other.RegionID;
            }
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals(obj as Source);
            }
            public override int GetHashCode()
            {
                return (this.SourceTypeID + this.RegionID + (this.FacilityName + this.FacilityCode).GetHashCode()).GetHashCode();
            }
        }//end 

    public partial class SourceType : IEquatable<SourceType>
    {
        public bool Equals(SourceType other)
        {
            return String.Equals(this.Name.ToLower(), other.Name.ToLower()) &&
                   String.Equals(this.Code.ToLower(), other.Code.ToLower());
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as SourceType);
        }
        public override int GetHashCode()
        {
            return (this.Name + this.Code).GetHashCode();
        }
    }//end 

    public partial class StatusType : IEquatable<StatusType>
    {
        public bool Equals(StatusType other)
        {
            return String.Equals(this.Name.ToLower(), other.Name.ToLower()) &&
                    String.Equals(this.Code.ToLower(), other.Code.ToLower());
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as StatusType);
        }
        public override int GetHashCode()
        {
            return (this.Name + this.Code).GetHashCode();
        }
    }//end 

    public partial class TimeSeries : IEquatable<TimeSeries>
    {
        public bool Equals(TimeSeries other)
        {
            return this.SourceID == other.SourceID && DateTime.Equals(this.Date,other.Date);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as TimeSeries);
        }
        public override int GetHashCode()
        {
            return (this.SourceID + this.Date.GetHashCode()).GetHashCode();
        }
    }//end 

    public partial class UnitType : IEquatable<UnitType>
    {
        public bool Equals(UnitType other)
        {
            return String.Equals(this.Name.ToLower(), other.Name.ToLower()) &&
                   String.Equals(this.Abbreviation.ToLower(), other.Abbreviation.ToLower());
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as UnitType);
        }
        public override int GetHashCode()
        {
            return (this.Name + this.Abbreviation).GetHashCode();
        }
    }//end 
}
