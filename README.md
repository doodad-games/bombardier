## Unusual Build Steps

- iOS, after building to Xcode...
    - the key-value storage capability must be added
        - Unity-iPhone.xcodeproj > Signing & Capabilities > (+ Capability > iCloud > Services >)_*_ Key-value storage
        - _*_ + Capability only needed if iCloud is not already visible under Signing & Capabilities
