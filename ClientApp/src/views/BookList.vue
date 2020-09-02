<template>
	<div>
		<el-button type="primary" @click="searchDialogVisible = true">查询书单</el-button>
		<el-scrollbar>
			<el-table height="calc(100vh - 149px)" stripe :data="booklist_pages" style="width: 100%">
				<el-table-column prop="id" label="书号">
					<template slot="header">
						<!-- <el-input v-model="book_filter.id" size="mini" placeholder="输入关键字搜索" /> -->
						<span>书号</span>
					</template>
					<template slot-scope="scope">{{scope.row.id}}</template>
				</el-table-column>
				<el-table-column prop="name" label="书单名称">
					<template slot="header">
						<!-- <el-input v-model="book_filter.name" size="mini" placeholder="输入关键字搜索" /> -->
						<span>书单名称</span>
					</template>
					<template slot-scope="scope">
						<span v-html="hightlight(scope.row.name,book_filter.name)"></span>
					</template>
				</el-table-column>
				<el-table-column prop="price" label="定价" width="70">
					<template slot="header">
						<!-- <el-input v-model="book_filter.price" size="mini" placeholder="输入关键字搜索" /> -->
						<span>定价</span>
					</template>
					<template slot-scope="scope">{{scope.row.price}}</template>
				</el-table-column>
				<el-table-column prop="price" label="折扣价" width="70" v-if="false">
					<template slot="header">
						<!-- <el-input v-model="book_filter.discount" size="mini" placeholder="输入关键字搜索" /> -->
						<span>折扣价</span>
					</template>
					<template slot-scope="scope">{{(scope.row.price * discount).toFixed(3)}}</template>
				</el-table-column>
				<el-table-column prop="type" label="书单类别">
					<template slot="header">
						<!-- <el-input v-model="book_filter.type" size="mini" placeholder="输入关键字搜索" /> -->
						<span>书单类别</span>
					</template>
					<template slot-scope="scope">
						<span v-html="hightlight(scope.row.type,book_filter.type)"></span>
					</template>
				</el-table-column>
				<el-table-column prop="author" label="作者">
					<template slot="header">
						<!-- <el-input v-model="book_filter.author" size="mini" placeholder="输入关键字搜索" /> -->
						<span>作者</span>
					</template>
					<template slot-scope="scope">
						<span v-html="hightlight(scope.row.author,book_filter.author)"></span>
					</template>
				</el-table-column>
				<el-table-column prop="translator" label="译者">
					<template slot="header">
						<!-- <el-input v-model="book_filter.translator" size="mini" placeholder="输入关键字搜索" /> -->
						<span>译者</span>
					</template>
					<template slot-scope="scope">{{scope.row.translator}}</template>
				</el-table-column>
				<el-table-column prop="isbn" label="ISBN分类">
					<template slot="header">
						<!-- <el-input v-model="book_filter.isbn" size="mini" placeholder="输入关键字搜索" /> -->
						<span>ISBN分类</span>
					</template>
					<template slot-scope="scope">
						<span v-html="hightlight(scope.row.isbn,book_filter.isbn)"></span>
					</template>
				</el-table-column>
				<el-table-column prop="version" label="出版社">
					<template slot="header">
						<!-- <el-input v-model="book_filter.version" size="mini" placeholder="输入关键字搜索" /> -->
						<span>出版社</span>
					</template>
					<template slot-scope="scope">
						<span v-html="hightlight(scope.row.version,book_filter.version)"></span>
					</template>
				</el-table-column>
				<el-table-column label="操作" width="120">
					<template slot-scope="scope">
						<el-popconfirm
							icon="el-icon-info"
							iconColor="red"
							title="确定要添加该图书到购物车吗？"
							@onConfirm="addtoCart(scope.$index, scope.row)"
						>
							<el-button
								size="mini"
								type="danger"
								slot="reference"
								:disabled="expire||(sumOfPrice+scope.row.price)>maxPrice"
							>加购物车</el-button>
						</el-popconfirm>
					</template>
				</el-table-column>
			</el-table>
			<el-pagination
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				:current-page="book_filter.page"
				:page-sizes="[10, 20, 50, 100, 200, 300, 400]"
				:page-size="book_filter.page_size"
				layout="total, sizes, prev, pager, next, jumper"
				:total="booklist_computed.length"
			></el-pagination>
		</el-scrollbar>

		<el-dialog title="书单查询" :visible.sync="searchDialogVisible">
			<el-form ref="book_filter" :model="book_filter" label-width="80px">
				<el-form-item label="书号" v-if="false">
					<el-input v-model="book_filter.id"></el-input>
				</el-form-item>
				<el-form-item label="书单名称">
					<el-input v-model="book_filter.name"></el-input>
				</el-form-item>
				<el-form-item label="定价">
					<el-col :span="11">
						<el-input type="number" v-model="book_filter.price_min"></el-input>
					</el-col>
					<el-col :span="2">~</el-col>
					<el-col :span="11">
						<el-input type="number" v-model="book_filter.price_max"></el-input>
					</el-col>
				</el-form-item>
				<el-form-item label="折扣价" v-if="false">
					<el-col :span="11">
						<el-input type="number" v-model="book_filter.discount_min"></el-input>
					</el-col>
					<el-col class="line" :span="2">~</el-col>
					<el-col :span="11">
						<el-input type="number" v-model="book_filter.discount_max"></el-input>
					</el-col>
				</el-form-item>
				<el-form-item label="书单类别">
					<el-input v-model="book_filter.type"></el-input>
				</el-form-item>
				<el-form-item label="作者">
					<el-input v-model="book_filter.author"></el-input>
				</el-form-item>
				<el-form-item label="译者" v-if="false">
					<el-input v-model="book_filter.translator"></el-input>
				</el-form-item>
				<el-form-item label="ISBN分类">
					<el-input v-model="book_filter.isbn"></el-input>
				</el-form-item>
				<el-form-item label="出版社">
					<el-input v-model="book_filter.version"></el-input>
				</el-form-item>
				<el-form-item>
					<el-button type="info" @click="searchDialogVisible = false">关闭</el-button>
				</el-form-item>
			</el-form>
		</el-dialog>
	</div>
</template>
<script>
	import { mapState, mapGetters } from "vuex";
	export default {
		data() {
			return {
				searchDialogVisible: false,
			};
		},
		computed: {
			...mapState(["book_filter"]),
			...mapGetters([
				"discount",
				"maxPrice",
				"booklist",
				"expire",
				"booklist_computed",
				"booklist_pages",
				"sumOfPrice",
			]),
		},
		methods: {
			search() {
				this.searchDialogVisible = true;
			},
			addtoCart(index, row) {
				console.log(index, row);
				const _this = this;
				this.$axios({
					method: "put",
					url: "api/book/add",
					data: {
						user: sessionStorage.getItem("user"),
						book: row.id,
					},
				}).then(function (res) {
					console.log(res);
					if (res.data.status == 200) {
						_this.$store.commit("addBook", { _book: row });
						_this.$message({
							message: "添加购物车成功！！！",
							type: "success",
						});
					} else {
						_this.$message({
							message: "添加购物车失败！！！",
							type: "error",
						});
					}
				});
			},
			handleSizeChange(val) {
				this.$store.commit("setBookFilterPageSize", val);
			},
			handleCurrentChange(val) {
				this.$store.commit("setBookFilterPage", val);
			},
			hightlight(val, filter) {
				const start = val.toString().indexOf(filter);
				const end = start + filter.length;
				return `${val
					.toString()
					.substr(
						0,
						start
					)}<b class="highlight">${filter}</b>${val
					.toString()
					.substr(end)}`;
			},
		},
		watch: {},
		filters: {},
	};
</script>

<style >
	.highlight {
		background: yellow;
	}
	/* .el-scrollbar {
			height: calc(100vh - 117px);
		}
		.el-scrollbar__wrap {
			overflow-x: hidden;
		} */
</style>