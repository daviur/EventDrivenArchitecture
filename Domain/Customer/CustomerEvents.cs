using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Customer;

public abstract record CustomerEvent;

public record CustomerCreated(string Name, string Email, string Phone) : CustomerEvent;

public record CustomerNameChanged(string Name) : CustomerEvent;

public record CustomerEmailChanged(string Email) : CustomerEvent;

public record CustomerPhoneChanged(string Phone) : CustomerEvent;
