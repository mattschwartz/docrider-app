// @flow
import { remote } from 'electron'
import React, { Component } from 'react'
import DocumentView from './DocumentView'

import homeStyles from '../styles/home.scss'
import DocinsContainer from './DocinsContainer'

type Props = {}

const __minimizeWindow = () => {
  const window = remote.getCurrentWindow()
  window.minimize()
}

const __maximizeWindow = () => {
  const window = remote.getCurrentWindow()
  if (!window.isMaximized()) {
    window.maximize()
  } else {
    window.unmaximize()
  }
}
const __closeWindow = () => {
  const window = remote.getCurrentWindow()
  window.close()
}

export default class HomePage extends Component<Props> {
  props: Props

  render () {
    return (
      <>
        <div className={homeStyles.docriderBanner}>
          <img
            className={homeStyles.brand}
            src='./dist/docrider-alt-banner.png'
            alt=''
          />
          <h1>Docrider</h1>
          <div className={homeStyles.windowControls}>
            <div className={homeStyles.btnGroup}>
              <button
                id='__windowButtonMinimize'
                role='minimize-window'
                type='button'
                onClick={__minimizeWindow}
              >
                <i className='fas fa-window-minimize' />
              </button>

              <button
                id='__windowButtonMaximize'
                role='maximize-window'
                type='button'
                onClick={__maximizeWindow}
              >
                <i className='fas fa-window-maximize' />
              </button>

              <button
                id='__windowButtonClose'
                role='close-window'
                type='button'
                onClick={__closeWindow}
              >
                <i className='fas fa-times' />
              </button>
            </div>
          </div>
        </div>

        <div className={homeStyles.docContainer}>
          <DocumentView />
          <DocinsContainer />
        </div>
      </>
    )
  }
}
