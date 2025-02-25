function FFGraph() {

	var game_id = 2;
	//var useBars = filterTimespan == 1 && filterItemLevelDisplay == "single"
	var useBars = true;
	var filterDataSet = 1000;
	var filterMetric = "speed";
	var filterTimespan = 1;
	var isBoxPlot = useBars && filterDataSet == 1000
	var filterItemLevelDisplay = "scaled";

	var tableColumns = [
		{ "sType": "inner-text" },
		{ "sType": "num-html" }, { "sType": "num-html" }, { "sType": "num-html" }
	];

	var sortOrder = 'desc';
	var sortColumn = 1;


	// FIXME: It's disgusting that we copied this from report.js. Move it somewhere where it can be shared.
	function setLineColorForActorFromType(type) {
		// We cannot use percentage rgb values or names, since they break fillOpacity for Highcharts for some weird reason.
		if (type == "Boss")
			return "#B4BDFF"
		if (type == "NPC")
			return "#84BDF8"
		if (type == "Pet")
			return "#64BDC8"

		if (game_id == 0) {
			if (type == "DeathKnight")
				return "rgb(196, 31, 59)"
			if (type == "DemonHunter")
				return "#a330c9"
			if (type == "Druid")
				return "rgb(255, 125, 10)"
			if (type == "Evoker")
				return "#33937F"
			if (type == "Hunter")
				return "rgb(171, 212, 115)"
			if (type == "Mage")
				return "rgb(105, 204, 240)"
			if (type == "Monk")
				return "#00ff98"
			if (type == "Paladin")
				return "rgb(245, 140, 186)"
			if (type == "Priest")
				return "#eeeeee"
			if (type == "Rogue")
				return "rgb(255, 245, 105)"
			if (type == "Shaman")
				return "rgb(36, 89, 255)"
			if (type == "Warlock")
				return "rgb(148, 130, 201)"
			if (type == "Warrior")
				return "rgb(199, 156, 110)"
			if (type == "TricksOfTheTrade" || type == "UnholyFrenzy")
				return "#2599be"
		} else if (game_id == 1) {
			if (type == "Medic")
				return "rgb(16, 130, 16)"
			if (type == "Stalker")
				return "rgb(209, 38, 204)"
			if (type == "Esper")
				return "#33e2d3"
			if (type == "Engineer")
				return "#d69c00"
			if (type == "Warrior")
				return "#cf2621"
			if (type == "Spellslinger")
				return "#2599be"
		} else if (game_id == 2) {
			if (type == "LimitBreak")
				return "#2599be"
			if (type == "Scholar")
				return "#8657FF"
			if (type == "Astrologian")
				return "#FFE74A"
			if (type == "Monk" || type == "Pugilist")
				return "#d69c00"
			if (type == "Ninja" || type == "Rogue")
				return "#AF1964"
			if (type == "WhiteMage" || type == "Conjurer")
				return "#FFF0DC"
			if (type == "DarkKnight")
				return "rgb(209, 38, 204)"
			if (type == "Paladin" || type == "Gladiator")
				return "#A8D2E6"
			if (type == "Bard" || type == "Archer")
				return "rgb(57%, 73%, 37%)"
			if (type == "Dragoon" || type == "Lancer")
				return "#4164CD"
			if (type == "Warrior" || type == "Marauder")
				return "#cf2621"
			if (type == "Machinist")
				return "#6EE1D6"
			if (type == "Summoner" || type == "Arcanist")
				return "#2D9B78"
			if (type == "BlackMage" || type == "Thaumaturge")
				return "#A579D6"
			if (type == "Samurai")
				return "#e46d04"
			if (type == "RedMage")
				return "#e87b7b"
			if (type == "BlueMage")
				return "rgb(36, 89, 255)"
			if (type == "Gunbreaker")
				return "#796D30"
			if (type == "Dancer")
				return "#E2B0AF"
			if (type == "Reaper")
				return "#965A90"
			if (type == "Sage")
				return "#80A0F0"
			if (type == "Viper")
				return "#108210"
			if (type == "Pictomancer")
				return "#fc92e1"
		} else if (game_id == 3) {
			if (type == "Mage")
				return "#A579D6"
			if (type == "Cleric")
				return "rgb(57%, 73%, 37%)"
			if (type == "Rogue")
				return "rgb(255, 245, 105)"
			if (type == "Warrior")
				return "#cf2621"
			if (type == "Primalist")
				return "#d69c00"
		} else if (game_id == 4) {
			if (type == "Nightblade")
				return "#e87b7b"
			if (type == "DragonKnight")
				return "rgb(255, 125, 10)"
			if (type == "Warden")
				return "rgb(16, 130, 16)"
			if (type == "Templar")
				return "rgb(255, 245, 105)"
			if (type == "Sorcerer")
				return "rgb(148, 130, 201)"
			if (type == "Necromancer")
				return "#a330c9"
			if (type == "Arcanist")
				return "rgb(171, 212, 115)";
		} else if (game_id == 5) {
			if (type == "JediSage" || type == "SithSorcerer")
				return "#A8D2E6"
			if (type == "SithAssassin" || type == "JediShadow")
				return "#4164CD"
			if (type == "SithJuggernaut" || type == "JediGuardian")
				return "rgb(58%, 51%, 79%)"
			if (type == "SithMarauder" || type == "JediSentinel")
				return "#cf2621"
			if (type == "Operative" || type == "Scoundrel")
				return "#d69c00"
			if (type == "Sniper" || type == "Gunslinger")
				return "#FFE74A"
			if (type == "Powertech" || type == "Vanguard")
				return "rgb(57%, 73%, 37%)"
			if (type == "Mercenary" || type == "Commando")
				return "#2D9B78"
		}

		// FIXME: It's gross to have the summary lines treated as "actors." Move this somewhere else.
		if (type == "Damage Done")
			return "#84BDF8"
		if (type == "Damage Taken")
			return "#F84D44"
		if (type == "Healing Done")
			return "#84f8bd"
		if (type == "Healing Received")
			return "rgb(225, 215, 105)"

		return "white"
	}

	var data = []
	var dataset = 1000;
	var nameForDataset = filterMetric == "deaths" ? "" : (dataset == 100 ? "최대" : (dataset == 0 ? "최소" : "#metric-dataset-" + dataset))
	var singleColumnSeries = { name: "점수", data: [], borderWidth: 0 };
	var singlePointSeries = { type: "scatter", name: "점수", data: [], borderWidth: 0 };
	var canHavePercentileSeries = false;
	var showingAbilitiesOrItems = false;

	function columnSort(a, b) {
		var aY = isBoxPlot && a.q3 !== undefined ? a.q3 : a.y
		var bY = isBoxPlot && b.q3 !== undefined ? b.q3 : b.y
		if (aY < bY)
			return filterMetric == "speed" || filterMetric == "ehrps" || (filterMetric == "deaths" && !showingAbilitiesOrItems) ? -1 : 1
		if (aY > bY)
			return filterMetric == "speed" || filterMetric == "ehrps" || (filterMetric == "deaths" && !showingAbilitiesOrItems) ? 1 : -1
		return 0
	}

	var series = { name: "음유시인", data: [] };
	series.color = setLineColorForActorFromType("Bard");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 74.243702614398;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 67.895006315128,
				q1: 71.170636982015,
				median: 73.117431993114,
				q3: q3,
				high: 75.465293900913
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 75.95860617717 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 74.243702614398 });
	} else
		series.data.push(74.243702614398);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "흑마도사", data: [] };
	series.color = setLineColorForActorFromType("BlackMage");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 87.429740194901;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 78.48930689464,
				q1: 80.715756045564,
				median: 84.376418361201,
				q3: q3,
				high: 91.486965516746
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 92.485612967812 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 87.429740194901 });
	} else
		series.data.push(87.429740194901);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "용기사", data: [] };
	series.color = setLineColorForActorFromType("Dragoon");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 86.646416565441;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 78.321019116341,
				q1: 82.681668300792,
				median: 85.41516210749,
				q3: q3,
				high: 89.266597678928
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 90.137895328066 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 86.646416565441 });
	} else
		series.data.push(86.646416565441);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "기공사", data: [] };
	series.color = setLineColorForActorFromType("Machinist");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 91.642664754444;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 82.927851947021,
				q1: 86.701608651363,
				median: 89.619201193041,
				q3: q3,
				high: 93.984138672153
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 94.290732197586 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 91.642664754444 });
	} else
		series.data.push(91.642664754444);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "몽크", data: [] };
	series.color = setLineColorForActorFromType("Monk");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 93.741519892491;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 88.538753865555,
				q1: 89.462328282616,
				median: 93.791494716978,
				q3: q3,
				high: 94.98168031208
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 95.240754275152 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 93.741519892491 });
	} else
		series.data.push(93.741519892491);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "닌자", data: [] };
	series.color = setLineColorForActorFromType("Ninja");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 87.478781599596;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 78.616917094769,
				q1: 82.248722188851,
				median: 87.058669325676,
				q3: q3,
				high: 90.274041176457
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 91.391541326205 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 87.478781599596 });
	} else
		series.data.push(87.478781599596);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "소환사", data: [] };
	series.color = setLineColorForActorFromType("Summoner");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 86.620100485711;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 78.081045979649,
				q1: 81.11967594887,
				median: 84.130770605765,
				q3: q3,
				high: 89.051657843961
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 89.985985965386 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 86.620100485711 });
	} else
		series.data.push(86.620100485711);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "적마도사", data: [] };
	series.color = setLineColorForActorFromType("RedMage");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 84.768888267837;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 77.185074840846,
				q1: 80.187887154379,
				median: 83.284169998904,
				q3: q3,
				high: 87.008109944383
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 87.643757195945 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 84.768888267837 });
	} else
		series.data.push(84.768888267837);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "사무라이", data: [] };
	series.color = setLineColorForActorFromType("Samurai");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 93.388600073357;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 85.667913605917,
				q1: 88.807997502947,
				median: 90.790277801051,
				q3: q3,
				high: 95.482138571393
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 96.630359494206 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 93.388600073357 });
	} else
		series.data.push(93.388600073357);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "무도가", data: [] };
	series.color = setLineColorForActorFromType("Dancer");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 75.922152417099;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 70.204796759009,
				q1: 72.019711799289,
				median: 74.111992657962,
				q3: q3,
				high: 78.027417392315
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 78.643918950786 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 75.922152417099 });
	} else
		series.data.push(75.922152417099);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "리퍼", data: [] };
	series.color = setLineColorForActorFromType("Reaper");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 86.577327131403;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 76.384822720178,
				q1: 80.509059359012,
				median: 83.883408458831,
				q3: q3,
				high: 89.704744175651
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 90.682549755817 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 86.577327131403 });
	} else
		series.data.push(86.577327131403);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "바이퍼", data: [] };
	series.color = setLineColorForActorFromType("Viper");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 89.372745085868;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 84.254696832918,
				q1: 86.686005029514,
				median: 88.851337105491,
				q3: q3,
				high: 89.747307948885
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 89.878526518712 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 89.372745085868 });
	} else
		series.data.push(89.372745085868);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);


	var series = { name: "픽토맨서", data: [] };
	series.color = setLineColorForActorFromType("Pictomancer");

	if (useBars) {
		var seriesColor = series.color;
		if (isBoxPlot) {
			var q3 = 94.936545796472;
			singleColumnSeries.data.push({
				color: seriesColor, name: series.name,
				low: 83.297253200081,
				q1: 86.76879934715,
				median: 90.203717110213,
				q3: q3,
				high: 96.891714389266
			});
			singlePointSeries.data.push({ color: seriesColor, name: series.name, q3: q3, y: 98.015714013948 });
		} else
			singleColumnSeries.data.push({ name: series.name, color: seriesColor, y: 94.936545796472 });
	} else
		series.data.push(94.936545796472);

	if (filterTimespan != 1) {
		series.pointStart = 1739880000 * 1000;
		series.pointInterval = 24 * 60 * 60 * 1000;
	}

	if (!useBars)
		data.push(series);



	if (useBars) {
		singleColumnSeries.data.sort(columnSort)
		data.push(singleColumnSeries)
		if (isBoxPlot) {
			singlePointSeries.data.sort(columnSort)
			data.push(singlePointSeries)
		}
	}

	// Now set up the graph.
	
	var xAxisDefaults = {
		type: useBars ? 'category' : (filterItemLevelDisplay == "scaled" ? "linear" : 'datetime'),
		title: {
			text: null
		}
	};

	console.log("test", xAxisDefaults);

	const chartOptions = {
		chart: {
			type: useBars ? (isBoxPlot ? 'boxplot' : 'bar') : 'line',
			renderTo: "graph",
			spacingRight: useBars ? 20 : 10,
			spacingLeft: useBars ? 20 : 0,
			borderWidth: 0,
			spacingTop: useBars ? 0 : 15,
			spacingBottom: 10,
			inverted: true
		},
		navigator: {
			enabled: false
		},
		scrollbar: {
			enabled: false
		},
		rangeSelector: {
			buttons: [],
			inputEnabled: false,
			enabled: true
		},
		title: {
			text: null
		},
		xAxis: xAxisDefaults,
		yAxis: {
			title: {
				text: "점수"
			},
			allowDecimals: true,
			endOnTick: true
		},
		tooltip: {
			enabled: true,
			followPointer: true,
			useHTML: true,
			shared: false,
			valueDecimals: 2,
			pointFormat: "<span style='font-size:10px'><span style='color:{point.color}'>{series.name}</span>: <b>{point.y}</b><br/></span>",
		},
		legend: {
			enabled: !useBars,
			align: "center",
			verticalAlign: "top",
			borderRadius: 0,
			borderColor: "#333333",
			backgroundColor: "black",
			floating: true,
			y: -17,
			padding: 5
		},
		plotOptions: {
			line: {
				allowPointSelect: false,
				animation: false
			},
			boxplot: {
				medianColor: 'rgba(0,0,0,0.3)',
				medianWidth: 2,
			}
		},
		series: data,
		credits: { enabled: false }
	};

	Highcharts.chart("ffgraph_1", chartOptions);
}