
namespace ERNST.Dto.WorkshopCoach
{
    public class WorkshopCoachForDisplay
    {
        public int Id { get; set; }

        public string CoachEnName { get; set; }

        public string CoachArName { get; set; }
        
        public int Available { get; set; }
        
        public int InUse { get; set; }
        
        public int InMaintenance { get; set; }
    }
}
