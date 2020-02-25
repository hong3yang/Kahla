﻿using Aiursoft.Handler.Models;
using System;
using System.Collections.Generic;

namespace Kahla.SDK.Models.ApiViewModels
{
    public class IndexViewModel : AiurProtocol
    {
        public string WikiPath { get; set; }
        public DateTime ServerTime { get; set; }
        public DateTime UTCTime { get; set; }
        public string APIVersion { get; set; }
        public string VapidPublicKey { get; set; }
        public string ServerName { get; set; }
        public string Mode { get; set; }
        public List<DomainSettings> Domains { get; set; }
    }
}
