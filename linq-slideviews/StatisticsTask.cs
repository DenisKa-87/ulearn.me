using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
	public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
	{
		if (visits.Count == 0 || !visits.Select(x => x.SlideType).Contains(slideType)) return 0; 
		var pairs = ExtensionsTask.Bigrams(visits.Select(x => x).OrderBy(x => x.UserId).ThenBy(x => x.DateTime));;
		var timeOnslide = pairs.Select(x => x)
			.Where(x => x.First.UserId == x.Second.UserId)
			.Select(x => (x.First.SlideId,x.First.SlideType,((x.Second.DateTime - x.First.DateTime).TotalSeconds/60)));
		var median = ExtensionsTask.Median(timeOnslide.Select(x => x).Where(x => x.SlideType.Equals(slideType))
			.Select(x => x.Item3).Where(x => x >= 1 && x <= 120));
		return median;
	}
}