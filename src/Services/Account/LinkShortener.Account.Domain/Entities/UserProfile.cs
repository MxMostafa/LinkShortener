﻿

using LinkShortener.Account.Domain.Entities.Base;

namespace LinkShortener.Account.Domain.Entities;

public class UserProfile : ActivateEntity<UserProfile>
{
    public User User { get; private set; } = null!;
    public long UserId { get; set; }

}
