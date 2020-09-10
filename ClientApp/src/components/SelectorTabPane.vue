<template>
	<div>
		<el-table
			:id="'seletor-tab-pane-'+floor"
			:data="allFloorBookCart(floor)"
			style="width: 100%"
			height="calc(100vh - 120px)"
		>
			<el-table-column prop="_user.organization" label="组织" width="60"></el-table-column>
			<el-table-column prop="_user.floor" label="楼层" width="60" v-if="!floor"></el-table-column>
			<el-table-column prop="_user.team_code" label="部门" width="60"></el-table-column>
			<el-table-column prop="_user.name" label="姓名" width="70"></el-table-column>
			<el-table-column prop="_book.id" label="书号" width="100">
				<template slot-scope="scope">
					<el-link
						type="primary"
						target="_blank"
						:href="'http://product.dangdang.com/'+scope.row._book.id+'.html'"
					>{{scope.row._book.id}}</el-link>
				</template>
			</el-table-column>
			<el-table-column prop="_book.name" label="自选" width="80">
				<template slot-scope="scope">
					<span v-if="scope.row.custom">
						<el-tag size="medium" type="success" effect="dark" v-if="scope.row.book<1000000000">自营</el-tag>
						<el-tag size="medium" type="danger" effect="dark" v-else>非自营</el-tag>
					</span>
				</template>
			</el-table-column>
			<el-table-column prop="_book.name" label="书名">
				<template slot-scope="scope">
					<span v-if="scope.row.custom">{{scope.row._book.name}}</span>
					<span v-else>{{scope.row._book.name}}</span>
				</template>
			</el-table-column>
			<el-table-column prop="_book.group_price" label="团购价" width="70">
				<template slot-scope="scope">
					<span v-if="scope.row.custom">{{(scope.row._book.price*0.69).toFixed(2)}}</span>
					<span v-else>{{scope.row._book.group_price}}</span>
				</template>
			</el-table-column>
			<el-table-column prop="_book.author" label="作者"></el-table-column>
			<el-table-column prop="_book.publisher" label="出版社"></el-table-column>
		</el-table>
		<el-button-group>
			<el-button type="info">总人数：{{ sumOfHuman(floor) }}</el-button>
			<el-button type="primary" @click="exportExcel">导出为Excel</el-button>
			<el-button type="info">总单数：{{ sumOfOrder(floor) }}</el-button>
		</el-button-group>
	</div>
</template>
<script>
	import FileSaver from "file-saver";
	import XLSX from "xlsx";
	import moment from "moment";
	import { mapState, mapGetters } from "vuex";
	export default {
		props: ["floor"],
		computed: {
			...mapState(["user"]),
			...mapGetters(["allBookCart", "allFloorBookCart"]),
			sumOfHuman() {
				return function (floor) {
					const humans = this.allFloorBookCart(floor).reduce(
						(array, value) => {
							array.push(value._user.job_number);
							return array;
						},
						[]
					);
					return [...new Set(humans)].length;
				};
			},
			sumOfOrder() {
				return function (floor) {
					return this.allFloorBookCart(floor).length;
				};
			},
		},
		methods: {
			exportExcel() {
				const floor = this.floor;
				/* generate workbook object from table */
				var wb = XLSX.utils.table_to_book(
					document.querySelector(`#seletor-tab-pane-${this.floor}`)
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
							(floor || "汇总") +
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
<style >
	.el-table .warning-row {
		background: #f56c6c;
		color: #fff;
	}
</style>