<template>
	<div id="app">
		<el-menu class="el-menu-demo" mode="horizontal" :router="true" :default-active="active">
			<el-menu-item index="/">首页</el-menu-item>
			<el-menu-item index="/booklist">书单</el-menu-item>
			<el-menu-item index="/shopping">我的购物车</el-menu-item>
			<el-menu-item
				index="/selectors"
				v-if="user.character=='floor admin'||user.character=='admin'||user.character=='system admin'"
			>选书名单</el-menu-item>
			<div
				v-if="this.$store.state.user.id"
				style="float:right;line-height:50px;margin-right:100px;"
			>{{user.organization}} {{user.team_code}} {{user.job_number}} {{user.name}}</div>
		</el-menu>
		<el-button-group style="float:right;margin-top:-55px;" v-if="this.$store.state.user.id">
			<el-button type="primary" @click="logout">登出</el-button>
		</el-button-group>
		<router-view></router-view>
		<el-dialog
			title="登录"
			:show-close="false"
			:close-on-press-escape="false"
			:close-on-click-modal="false"
			:visible.sync="dialogVisible"
		>
			<el-input v-model="user.job_number" placeholder="请输入工号登录">
				<template slot="prepend">C-</template>
			</el-input>
			<!-- <span style="color:#F56C6C;" v-if="this.$store.state.user.id">{{message}}</span> -->
			<span slot="footer" class="dialog-footer">
				<el-button type="primary" @click="login">确 定</el-button>
			</span>
		</el-dialog>
	</div>
</template>

<script>
	import { mapState } from "vuex";
	export default {
		name: "app",
		components: {},
		data() {
			return {
				active: "/",
				dialogVisible: true,
				bookDialogVisible: false,
				input: "",
				message: "很抱歉，您未参与本次活动，如有错误，请联系负责人！",
				job_number: "",
			};
		},
		created() {
			if (sessionStorage.getItem("user")) {
				this.dialogVisible = false;
				this.$store.dispatch("login", sessionStorage.getItem("user"));
				this.$store.dispatch("getConfig");
				this.$store.dispatch("getShopping", sessionStorage.getItem("user"));
			} else {
				this.dialogVisible = true;
			}
		},
		mounted() {},
		computed: {
			...mapState(["user"]),
		},
		methods: {
			login: function () {
				this.$store.dispatch("login", "C-" + this.user.job_number);
			},
			logout: function () {
				sessionStorage.removeItem("user");
				this.$router.push("/");
				location.reload();
			},
		},
		watch: {
			$route(to) {
				this.active = to.path;
			},
			job_number: function () {
				this.message = "";
			},
		},
	};
</script>

<style>
	#app {
		font-family: "Avenir", Helvetica, Arial, sans-serif;
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;
		text-align: center;
		color: #2c3e50;
	}
</style>
