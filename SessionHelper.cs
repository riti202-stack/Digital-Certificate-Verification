using System;
using System.Web;

public static class SessionHelper
{
    public static int UserId
    {
        get { return Convert.ToInt32(HttpContext.Current.Session["UserId"]); }
        set { HttpContext.Current.Session["UserId"] = value; }
    }

    public static string UserName
    {
        get { return HttpContext.Current.Session["UserName"] as string; }
        set { HttpContext.Current.Session["UserName"] = value; }
    }

    public static string Role
    {
        get { return HttpContext.Current.Session["Role"] as string; }
        set { HttpContext.Current.Session["Role"] = value; }
    }

    public static string RollNumber
    {
        get { return HttpContext.Current.Session["RollNumber"] as string; }
        set { HttpContext.Current.Session["RollNumber"] = value; }
    }

    public static bool IsLoggedIn => HttpContext.Current.Session["UserId"] != null;

    public static void RequireRole(string requiredRole)
    {
        if (!IsLoggedIn || Role != requiredRole)
        {
            HttpContext.Current.Response.Redirect("~/Login.aspx");
        }
    }

    public static void Logout()
    {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
    }
}