// @flow
import React, { Component } from 'react'
import DocumentView from './DocumentView'

import homeStyles from '../styles/home.scss'

type Props = {}

export default class HomePage extends Component<Props> {
  props: Props

  render () {
    return (
      <>
        <div className={homeStyles.docriderBanner}>
          <img
            className={homeStyles.brand}
            src='./dist/docrider-alt-banner.png'
          />
          <h1>Docrider</h1>
          <div className={homeStyles.windowControls}>
            <div className={homeStyles.btnGroup}>
              <button type='button'>
                <i className='fas fa-window-minimize' />
              </button>

              <button type='button'>
                <i className='fas fa-window-maximize' />
              </button>

              <button type='button'>
                <i className='fas fa-times' />
              </button>
            </div>
          </div>
        </div>
        <DocumentView />
      </>
    )
  }
}
