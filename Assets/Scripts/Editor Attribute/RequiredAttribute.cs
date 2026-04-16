using UnityEngine;

public class RequiredAttribute : PropertyAttribute 
{
    public string ErrorMessage;

    public RequiredAttribute(string errorMessage = "Field required!") 
    {
        this.ErrorMessage = errorMessage;
    }
}