﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 03/06/2020 20:47:53
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;

namespace SavacFarma_Shared
{
    public partial class UserProcess {

        public UserProcess()
        {
            OnCreated();
        }

        public virtual int IdUser
        {
            get;
            set;
        }

        public virtual int IdProcess
        {
            get;
            set;
        }

        public virtual Usuario Usuario
        {
            get;
            set;
        }

        public virtual Process Process
        {
            get;
            set;
        }

        #region Extensibility Method Definitions

        partial void OnCreated();

        public override bool Equals(object obj)
        {
          UserProcess toCompare = obj as UserProcess;
          if (toCompare == null)
          {
            return false;
          }

          if (!Object.Equals(this.IdUser, toCompare.IdUser))
            return false;
          if (!Object.Equals(this.IdProcess, toCompare.IdProcess))
            return false;

          return true;
        }

        public override int GetHashCode()
        {
          int hashCode = 13;
          hashCode = (hashCode * 7) + IdUser.GetHashCode();
          hashCode = (hashCode * 7) + IdProcess.GetHashCode();
          return hashCode;
        }

        #endregion
    }

}
