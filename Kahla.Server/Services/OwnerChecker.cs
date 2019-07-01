﻿using Aiursoft.Pylon.Exceptions;
using Aiursoft.Pylon.Models;
using Kahla.Server.Data;
using Kahla.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Kahla.Server.Services
{
    public class OwnerChecker
    {
        private readonly KahlaDbContext _dbContext;

        public OwnerChecker(KahlaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GroupConversation> FindMyOwnedGroupAsync(string groupName, string userId)
        {
            var group = await _dbContext.GroupConversations.SingleOrDefaultAsync(t => t.GroupName == groupName);
            if (group == null)
            {
                throw new AiurAPIModelException(ErrorType.NotFound, $"We can not find a group with name: '{groupName}'!");
            }
            if (group.OwnerId != userId)
            {
                throw new AiurAPIModelException(ErrorType.Unauthorized, $"You are not the owner of this group: '{groupName}' and you can't transfer it!");
            }
            return group;
        }
    }
}
