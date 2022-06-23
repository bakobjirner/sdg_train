using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Role
{
    private string role;
    private string objective;

    public Role(string role, string objective)
    {
        this.role = role;
        this.objective = objective;
    }

    public string getRole()
    {
        return role;
    }
    public string getObjective()
    {
        return objective;
    }
}
