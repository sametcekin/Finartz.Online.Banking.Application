﻿namespace Domain.Exceptions;

public class BusinessRuleException : Exception
{
    public BusinessRuleException()
    {

    }
    public BusinessRuleException(string message) : base(message)
    {

    }
}
