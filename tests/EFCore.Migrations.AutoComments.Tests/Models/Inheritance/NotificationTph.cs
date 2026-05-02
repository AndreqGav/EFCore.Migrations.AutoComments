namespace EFCore.Migrations.AutoComments.Tests.Models.Inheritance;

/// <summary>
/// Base notification.
/// </summary>
public class NotificationBase
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Discriminator.
    /// </summary>
    public string Discriminator { get; set; }
}

/// <summary>
/// Notification sent via an SMS gateway.
/// </summary>
public class SmsNotification : NotificationBase
{
    /// <summary>
    /// Message text to send.
    /// </summary>
    public string Content { get; set; }
}

/// <summary>
/// Email notification.
/// </summary>
public class EmailNotification : NotificationBase
{
    /// <summary>
    /// Message text to send.
    /// </summary>
    public string Content { get; set; }
}

/// <summary>
/// System notification about service operation.
/// </summary>
public class SystemNotification : NotificationBase
{
    /// <summary>
    /// System event code (INFO, WARN, ERROR).
    /// </summary>
    public string Content { get; set; }
}