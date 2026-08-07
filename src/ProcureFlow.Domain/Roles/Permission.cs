using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Entities;

namespace ProcureFlow.Domain.Roles
{
    public class Permission : BaseEntity
    {

        public string Code { get; private set; } = null!;

        public string Name { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        private Permission()
        {

        }

        public Permission(string code, string name, string description)
        {
            Code = code.Trim();
            Name = name.Trim();
            Description = description.Trim();
        }
    }
}
