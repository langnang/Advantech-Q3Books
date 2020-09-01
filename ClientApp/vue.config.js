module.exports = {
    devServer: {
        port: 19103,// 自定义端口
    },
    chainWebpack: config => {
        config
            .plugin('html')
            .tap(args => {
                args[0].title = '书香A家'
                return args
            })
    }
}