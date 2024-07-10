using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginModel
{
    public int status { get; set; }
    public string username { get; set; }
    public string notification { get; set; }
    public LoginModel(int status, string username, string notification)
    {
        this.status = status;
        this.username = username;
        this.notification = notification;
    }   
}
