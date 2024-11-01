const http = require("http");
const EventEmitter = require("events");
const PORT = process.env.PORT || 5000;
const fs = require("fs");

const emitter = new EventEmitter();

class Router {
    constructor() {
        this.endpoints = {}
    }

    request(method = "GET", path, handler) {
        if(!this.endpoints[path]) {
            this.endpoints[path] = {};
        }

        const endpoint = this.endpoints[path];

        if(endpoint[method]) {
            throw new Error(`[${method}] по адресу ${path} уже существует`)
        }

        endpoint[method] = handler;
        emitter.on(`[${path}]:[${method}]`, (req, res) => {
            handler(req, res);
        })
    }

    get(path, handler) {
        this.request('GET', path, handler);
    }
    post(path, handler) {
        this.request('POST', path, handler);
    }
    put(path, handler) {
        this.request('PUT', path, handler);
    }
    delete(path, handler) {
        this.request('DELETE', path, handler);
    }
}

const router = new Router();

router.get("/users", (req, res) => {
    res.end('YOU SEND REQUEST TO /USERS');
})

router.get("/posts", (req, res) => {
    res.end('YOU SEND REQUEST TO /POSTS');
})

const server = http.createServer((req, res) => {
    // res.writeHead(200, {
    //     "Content-Type": "text/html; charset=utf-8", });
    // if (req.url === '/us')
    // {
    //     return res.end('US1')
    // }

    // const emitted = emitter.emit(`[${req.url}]:[${req.method}]`, req, res);
    // if (!emitted) {
    //     res.end(req.url);
    // }

    //res.end(req.url);
    const filePath = req.url.substring(1);
    fs.readFile(filePath, function(error, data) {

        if (error) {
            res.statusCode = 404;
            res.end("Resourse not found!");
        } else {
            res.end(data);
        }
    });
})

server.listen(PORT, () => console.log(`Listening on ${PORT}`));