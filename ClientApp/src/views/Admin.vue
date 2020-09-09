<template>
	<el-tabs tab-position="left">
		<el-tab-pane label="书单管理">
			<el-table :data="allBookCart" style="width: 100%">
				<el-table-column prop="_user.organization" label="组织" width="60"></el-table-column>
				<el-table-column prop="_user.team_code" label="部门" width="60"></el-table-column>
				<el-table-column prop="_user.job_number" label="工号" width="80"></el-table-column>
				<el-table-column prop="_user.name" label="姓名" width="70"></el-table-column>
				<el-table-column prop="_book.id" label="书号" width="70"></el-table-column>
				<el-table-column prop="_book.name" label="书名"></el-table-column>
				<el-table-column prop="_book.price" label="团购价" width="70"></el-table-column>
				<el-table-column prop="_book.author" label="作者"></el-table-column>
				<el-table-column prop="_book.version" label="出版社"></el-table-column>
			</el-table>
		</el-tab-pane>
		<el-tab-pane label="程序设置">程序设置</el-tab-pane>
	</el-tabs>
</template>

<script>
	import { mapGetters } from "vuex";
	export default {
		created() {
			this.$store.dispatch("getBooks");
			this.$store.dispatch("getUsers");
		},
		computed: {
			...mapGetters(["allBookCart"]),
		},
		methods: {
			objectSpanMethod({ row, column, rowIndex, columnIndex }) {
				console.log(row, column, rowIndex, columnIndex);
				return {
					rowspan: 1,
					colspan: 1,
				};
			},
		},
	};
</script>