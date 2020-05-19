import * as yargs from 'yargs';
import {Argv} from 'yargs';
import * as fs from 'fs';
import { UpdateHandler } from './updateHandler';

let argv = yargs
    .command('update', 'update json content', (args: Argv) => {
        return yargs.option('file', {
            describe: 'file to update',
            requiresArg: true
        });
    }).argv;

switch (argv._[0]) {
    case 'update': {
        var handler = new UpdateHandler();
        handler.handle(argv);
    }
    break;
    default: {
        console.log(`unknown command : ${argv._}`);
    }
}