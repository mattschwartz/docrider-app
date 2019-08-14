import React from 'react'

import styles from '../../styles/docins/docins.outline.scss'

class OutlineDocin extends React.Component {
  render () {
    return (
      this.props.isVisible && (
        <div className={styles.charactersDocin}>
          <p>Outline:</p>
          <ul>
            <li>Chapter 1</li>
            <li>Chapter 2</li>
          </ul>
        </div>
      )
    )
  }
}

export default OutlineDocin
