public class UserState
{
    // Store the user's details
    public User? CurrentUser { get; private set; }

    // Method to set the current user
    public void SetCurrentUser(User user)
    {
        CurrentUser = user;
    }

    // Clear the current user details (e.g., on logout)
    public void ClearUser()
    {
        CurrentUser = null;
    }
}

public class User
{
    public string EmpCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Department { get; set; }
}
