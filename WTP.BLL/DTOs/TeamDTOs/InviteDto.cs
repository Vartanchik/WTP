namespace WTP.BLL.DTOs.TeamDTOs
{
    public class InviteDto
    {
        public InviteDto(int playerId, int teamId, bool invitation)
        {
            PlayerId = playerId;
            TeamId = teamId;
            Invitation = invitation;
        }

        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public bool Invitation { get; set; }
    }
}
