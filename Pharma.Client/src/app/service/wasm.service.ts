import { Injectable } from '@angular/core'
import { Observable, BehaviorSubject } from 'rxjs'


@Injectable()
export class WasmService {
  module: any

  wasmReady = new BehaviorSubject<boolean>(false)

  // constructor() {
  //   this.instantiateWasm('a.wasm')
  // }

  // async instantiateWasm(url: string) {

  //   const wasmFile = await fetch(url)
  //   const buffer = await wasmFile.arrayBuffer()
  //   const binary = new Uint8Array(buffer)
  //   const moduleArgs = {
  //     wasmBinary: binary,
  //     onRuntimeInitialized: () => {
  //       this.wasmReady.next(true)
  //     },
  //   }
  // }


}

