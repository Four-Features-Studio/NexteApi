using Newtonsoft.Json;
using NexteAPI.Entity;
using System.Collections.Generic;

namespace NexteAPI.DTO.FilesResponse
{
    public class ClientFilesResponse
    {
        [JsonProperty("typeHash")]
        public ChecksumMethod TypeHash { get; set; }

        [JsonProperty("files")]
        public List<FileEntity> Files { get; set; }
    }
}
