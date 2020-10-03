-- get identity users
select u.UserName, r.[Name] UserRole
from AspNetUsers u
	left join AspNetUserRoles ur on ur.UserId = u.Id
	left join AspNetRoles r on r.Id = ur.RoleId
order by r.[Name], u.UserName