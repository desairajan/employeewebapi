using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWebApi.Enums
{
    public enum RejectionCode
    {
        RC1, // RequestId should be unique
        RC2, // Transaction not found in request
        RC3, // Unauthorized api access
        RC4, // Api access token should be provided
        RC5, // Unexpection exception has occurred with detail - 
        RC6, // Transactional level error
        RC7, // Employee Firstname is mandatory
        RC8, // Employee Pan is mandatory
        RC9, // BirthDate should be in format dd-MM-yyyy
        RC10, //Employee already exists
        RC11 // Employee doesnt exists
    }
}