<template>
	<div>
		<el-tabs tab-position="left" :value="user.floor">
			<el-tab-pane
				name="2F"
				label="2F"
				:disabled="user.floor!='2F'&&user.character!='admin'&&user.character!='system admin'"
			>
				<SelectorTabPane floor="2F" />
			</el-tab-pane>
			<el-tab-pane
				name="3F"
				label="3F"
				:disabled="user.floor!='3F'&&user.character!='admin'&&user.character!='system admin'"
			>
				<SelectorTabPane floor="3F" />
			</el-tab-pane>
			<el-tab-pane
				name="4F南"
				label="4F南"
				:disabled="user.floor!='4F南'&&user.character!='admin'&&user.character!='system admin'"
			>
				<SelectorTabPane floor="4F南" />
			</el-tab-pane>
			<el-tab-pane
				name="4F北"
				label="4F北"
				:disabled="user.floor!='4F北'&&user.character!='admin'&&user.character!='system admin'"
			>
				<SelectorTabPane floor="4F北" />
			</el-tab-pane>
			<el-tab-pane
				name="5F"
				label="5F"
				:disabled="user.floor!='5F'&&user.character!='admin'&&user.character!='system admin'"
			>
				<SelectorTabPane floor="5F" />
			</el-tab-pane>

			<el-tab-pane name="总价" label="总价">
				<el-table
					:id="'seletor-tab-pane-总价'"
					:data="sumOfHumanPrice"
					style="width: 100%"
					height="calc(100vh - 120px)"
					:default-sort="{prop: 'sum', order: 'descending'}"
				>
					<el-table-column prop="organization" label="组织"></el-table-column>
					<el-table-column prop="floor" label="楼层"></el-table-column>
					<el-table-column prop="team_code" label="部门"></el-table-column>
					<el-table-column prop="job_number" label="工号"></el-table-column>
					<el-table-column prop="name" label="姓名"></el-table-column>
					<el-table-column prop="sum" label="书数">
						<template slot-scope="scope">{{scope.row.books.length}}</template>
					</el-table-column>
					<el-table-column prop="sum" label="总价" sortable>
						<template slot-scope="scope">{{(scope.row.sum).toFixed(2)}}</template>
					</el-table-column>
				</el-table>
				<el-button-group>
					<el-button type="info">总人数：{{ sumOfHumanPrice.length }}</el-button>
					<el-button type="primary" @click="exportExcel">导出为Excel</el-button>
					<el-button type="info">总价格：{{ sumOfHumanPrice.reduce((sum,v)=>sum+v.sum,0).toFixed(2) }}</el-button>
				</el-button-group>
			</el-tab-pane>
			<el-tab-pane name="汇总" label="汇总">
				<SelectorTabPane />
			</el-tab-pane>
		</el-tabs>
	</div>
</template>
<script>
	import FileSaver from "file-saver";
	import XLSX from "xlsx";
	import moment from "moment";
	import { mapState, mapGetters } from "vuex";
	import SelectorTabPane from "./../components/SelectorTabPane.vue";
	export default {
		components: {
			SelectorTabPane,
		},
		created() {
			this.$store.dispatch("getAllBooks");
		},
		computed: {
			...mapState(["user"]),
			...mapGetters(["sumOfHumanPrice"]),
		},
		methods: {
			exportExcel() {
				/* generate workbook object from table */
				var wb = XLSX.utils.table_to_book(
					document.querySelector(`#seletor-tab-pane-总价`)
				);
				/* get binary string as output */
				var wbout = XLSX.write(wb, {
					bookType: "xlsx",
					bookSST: true,
					type: "array",
				});
				try {
					FileSaver.saveAs(
						new Blob([wbout], { type: "application/octet-stream" }),
						"书香A家-" +
							"总价" +
							"-" +
							moment().format("YYYYMMDDHHmmss") +
							".xlsx"
					);
				} catch (e) {
					if (typeof console !== "undefined") console.log(e, wbout);
				}
				return wbout;
			},
		},
	};
</script>

<style></style>
