using Microsoft.AspNetCore.Identity;

namespace UrlApp.Data;

public class XUser :IdentityUser<Guid>
{
    public XUser() { }
    public DateTime CreatedDate { get; set; }
    public string Name { get; set; }
}


public class XRole : IdentityRole<Guid>
{ 
}

